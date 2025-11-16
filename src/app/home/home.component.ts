import { Component, effect, ElementRef, inject, QueryList, signal, ViewChildren } from '@angular/core';
import { DataService } from '../data.service';
import { TableBarComponent } from "../table-bar/table-bar.component";
import { FormsModule } from '@angular/forms';
import { OrderDto } from '../swagger';
import { MyOrderDto } from '../shared/MyOrderDto';
@Component({
  selector: 'app-home',
  imports: [TableBarComponent, FormsModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})

export class HomeComponent {

  @ViewChildren('addOrderRow') addOrderRow!: QueryList<ElementRef>;

  PageButtonClicked(val: number) {

    if (this.dataService.pageNumber() + val >= 1)
      this.dataService.pageNumber.update((x) => x += val);
    this.dataService.refreshOrdersList();
  }
  selectedOrderBillDatechanged(billCreatedDate: string, order: MyOrderDto) {
    if (billCreatedDate === undefined || billCreatedDate === null) return;
    let splittedDate = billCreatedDate.split('.');
    if (splittedDate[2].length === 4 && splittedDate[1].length === 2 && splittedDate[0].length === 2) {
      order.billCreatedDate = `${splittedDate[2]}-${splittedDate[1]}-${splittedDate[0]}`
      this.changeSelectedOrder(order);
    } else {
      console.log("Date-format ERROR: format 'dd.MM.yyyy'")
    }
  }
  private errorTimeoutId: ReturnType<typeof setTimeout> | null = null;
  private messageTimeoutId: ReturnType<typeof setTimeout> | null = null;

  ShowMessageTxt(txt: string) {
    this.messageText.set(txt);
    if (this.messageTimeoutId) {
      clearTimeout(this.messageTimeoutId);
    }
    this.messageTimeoutId = setTimeout(() => {
      this.messageText.set('');
      this.messageTimeoutId = null;
    }, 5000);
  }
  ShowErrorTxt(txt: string) {
    this.errorText.set(txt);
    if (this.errorTimeoutId) {
      clearTimeout(this.errorTimeoutId);
    }
    this.errorTimeoutId = setTimeout(() => {
      this.errorText.set('');
      this.errorTimeoutId = null;
    }, 5000);
  }
  OpenPdf(po: string | null, bill: string | null, billCreatedDate: string | null) {
    if (po !== null && po !== undefined || bill !== null && billCreatedDate !== null && bill !== undefined && billCreatedDate !== undefined) {
      this.dataService.orderService.ordersOpenPdfPost(po!, bill!, billCreatedDate!).subscribe({
        next: (response) => { console.log("PDF-Opened successfully!"); this.ShowMessageTxt("PDF-Opened successfully!") }, error: (err) => {

          console.log("Error in OrdersOpenPdfPost: ", err);
          this.ShowErrorTxt("Error on opening PDF: " + err.error.message);
        },
      });
    }
    this.ShowErrorTxt("No bill nor billCreatedDate found!");

  }
  ngAfterViewChecked() {
    if (this.addOrderRow && this.addOrderRow.length > 0) {
      this.addOrderRow.last.nativeElement.scrollIntoView({ behavior: 'smooth' });
    }
  }
  AddNewOrder() {
    if (!this.addNewOrder()) {
      this.ngAfterViewChecked();
      this.addNewOrder.set(true);
      this.newOrder().documentNr = this.GetGreatestDocumentNr() + 1;
      this.newOrder().uid = "GB 877 714 869";
      return;
    }
    if (!this.newOrder().inputDate?.match(/\d{2}\.\d{2}\.\d{4}/)) {
      this.ShowErrorTxt("Eingangsdatum ist nicht im Format: dd.MM.yyyy")
      return;
    }

    this.newOrder().billCreatedDate = null!;
    if (this.newOrder().deliveryDate?.match(/\d{2}\.\d{2}\.\d{4}/))
      this.newOrder().deliveryDate = this.ParseDateForBackend(this.newOrder().deliveryDate);
    else { this.newOrder().deliveryDate = null! }

    this.newOrder().inputDate = this.ParseDateForBackend(this.newOrder().inputDate);
    this.newOrder().netto = this.CalcNetto();
    if (this.newOrder().paymentDate?.match(/\d{2}\.\d{2}\.\d{4}/))
      this.newOrder().paymentDate = this.ParseDateForBackend(this.newOrder().paymentDate);
    else { this.newOrder().paymentDate = null! }
    this.newOrder().id = -1;
    console.log(JSON.stringify(this.newOrder(), null, 2));



    this.dataService.orderService.ordersPost(this.newOrder())
      .subscribe({
        next: (response) => {
          console.log("Order added succesfully: "); this.ShowMessageTxt("Order added succesfully!"); this.dataService.refreshOrdersList(); this.addNewOrder.set(false); this.newOrder.set({ isSelected: false, inputDate: this.GetDateToday(), status: 'In Arbeit', netto: 0, });
        },
        error: (error) => {
          console.log("Error in PostOrder: ", error.error.message); this.newOrder().inputDate = this.ParseDateForFrontend(this.newOrder().inputDate); if (this.newOrder().deliveryDate !== undefined || this.newOrder().deliveryDate !== null) this.newOrder().deliveryDate = this.ParseDateForFrontend(this.newOrder().deliveryDate); this.ShowErrorTxt("Error: " + "Check your input!");
        }
      })
  }
  GetDateToday(): string {
    return this.ParseDateForFrontend(new Date().toISOString());
  }
  addNewOrder = signal<boolean>(false);
  messageText = signal<string>(null!);
  errorText = signal<string>(null!);
  selectAll = signal<boolean>(false);
  newOrder = signal<MyOrderDto>({ isSelected: false, inputDate: this.GetDateToday(), status: 'In Arbeit', netto: 0, uid: "GB 877 714 869" });

  GetGreatestDocumentNr(): number {
    return this.dataService.orders().max(x => x.documentNr!);
  }
  deleteOrders() {
    let orderIds: number[] = [];
    this.dataService.GetSelectedOrders().forEach(x => orderIds.push(x.id!));
    this.dataService.orderService.ordersDelete(orderIds).subscribe(x => {
      console.log("Orders successfully deleted!"); this.dataService.refreshOrdersList();
      this.selectAll.set(false);
    });

  }

  selectedOrderDeliveryDatechanged(deliveryDate: string, order: MyOrderDto) {
    if (deliveryDate === undefined || deliveryDate === null) return;

    let splittedDate = deliveryDate.split('.');
    if (splittedDate[2].length === 4 && splittedDate[1].length === 2 && splittedDate[0].length === 2) {
      order.deliveryDate = `${splittedDate[2]}-${splittedDate[1]}-${splittedDate[0]}`
      this.changeSelectedOrder(order);
    } else {
      console.log("Date-format ERROR: format 'dd.MM.yyyy'")
    }
  }
  selectedOrderPaymentDatechanged(paymentDate: string, order: MyOrderDto) {
    if (paymentDate === undefined || paymentDate === null) return;

    let splittedDate = paymentDate.split('.');
    if (splittedDate[2].length === 4 && splittedDate[1].length === 2 && splittedDate[0].length === 2) {
      order.paymentDate = `${splittedDate[2]}-${splittedDate[1]}-${splittedDate[0]}`
      this.changeSelectedOrder(order);
    }
    else {
      console.log("Date-format ERROR: format 'dd.MM.yyyy'")
    }
  }
  selectedOrderInputDatechanged(inputDate: string, order: MyOrderDto) {
    if (inputDate === undefined || inputDate === null) return;

    let splittedDate = inputDate.split('.');
    if (splittedDate[2].length === 4 && splittedDate[1].length === 2 && splittedDate[0].length === 2) {
      order.inputDate = `${splittedDate[2]}-${splittedDate[1]}-${splittedDate[0]}`
      this.changeSelectedOrder(order);
    } else {
      console.log("Date-format ERROR: format 'dd.MM.yyyy'")
    }
  }
  changeSelectedOrder(selectedOrder: MyOrderDto) {
    if (selectedOrder.tax !== 0) {
  selectedOrder.netto = Number(((selectedOrder!.brutto ?? 0) / (1 + (selectedOrder.tax ?? 0) / 100)).toFixed(2));
  selectedOrder.brutto = Number((selectedOrder.netto! * (1 + (selectedOrder.tax! / 100))).toFixed(2));
}
    this.dataService.orderService.ordersOldOrderIdPut(selectedOrder.id!, selectedOrder).subscribe({
      next: (response) => {
        console.log("Order updated succesfully: "); this.ShowMessageTxt("Order updated succesfully!"); this.addNewOrder.set(false);
      },
      error: (error: Error) => {
        console.log("Error in PutOrder: ", error); this.ShowErrorTxt("Überprüfe deine Eingabe!");
      }
    });
  }

  ParseDateForFrontend(date: string | undefined | null): string {
    if (date === undefined || date === null) return "";
    let splittedDate = date?.split('-');
    return `${splittedDate[2].split('T')[0]}.${splittedDate[1]}.${splittedDate[0]}`
  }
  ParseDateForBackend(date: string | undefined): string {
    if (date === undefined) return null!;
    let splittedDate = date?.split('.');
    return `${splittedDate[2]}-${splittedDate[1]}-${splittedDate[0]}`
  }


  ChangeStatus(selectedOrder: OrderDto, status: string) {
    const selectedOrders = this.dataService.GetSelectedOrders();

    if (selectedOrders.length <= 0) {
      console.log("In Change status selected >=0");
      if (status === "Erledigt") {
        console.log("Erledigt true");
        selectedOrder.deliveryDate = new Date().toISOString();
      } else if (status === "Bezahlt") {
        console.log("Bezahlt true");
        selectedOrder.paymentDate = new Date().toISOString();
      }

      selectedOrder.status = status;
      this.dataService.orderService.ordersStatusPut([selectedOrder]).subscribe();
      return;
    }
    console.log("In Change status other");
    selectedOrders.forEach(x => x.status = status);
    this.dataService.orderService.ordersStatusPut(selectedOrders).subscribe();

  }
  ordersCheckboxClicked(isChecked: boolean) {
    this.dataService.orders()?.forEach(x => x.isSelected = isChecked);
    this.selectAll.set(true);
  }

  dataService = inject(DataService);
  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;

    if (input.files && input.files.length > 0) {
      const file = input.files[0];
      const reader = new FileReader();
      reader.onload = () => {

        const content = reader.result as string;
        if (content.startsWith("Job no.,")) {
          const lines = content.split('\n').splice(1);
          console.log("Lines of General ", lines);
          this.dataService.orderService.ordersCreatePdfPost(lines).subscribe({
            next: (response) => {
              console.log("PDF succesfully created!"); this.ShowMessageTxt("PDF succesfully created!!"); this.dataService.refreshOrdersList();
            },
            error: (error: Error) => {
              console.log("Error in ordersImportCsvPost: ", error); this.ShowErrorTxt("Error in reading orders from csv! (Check file content)");
            }

          });
        } else {
          const lines = content.split('\n').splice(3);
          console.log("Lines ", lines);
          this.dataService.orderService.ordersImportCsvPost(lines).subscribe({
            next: (response) => {
              console.log("Orders succesfully added!"); this.ShowMessageTxt("Orders successfully added!"); this.dataService.refreshOrdersList();
            },
            error: (error: Error) => {
              console.log("Error in ordersImportCsvPost: ", error); this.ShowErrorTxt("Error in reading orders from csv! (Check file content)");
            }
          })
        };
      }
      reader.readAsText(file);
      reader.onerror = (error) => console.error('Error reading file:', error);
    }
    input.value = null!;
  }

  openFileExplorer(fileInput: HTMLInputElement): void {
    fileInput.click();
  }
  CalcNetto(): number {
    if (this.newOrder() === null || this.newOrder() === undefined) return 0;
    return ((this.newOrder()!.brutto ?? 0) - ((this.newOrder().brutto ?? 0) *
      ((this.newOrder().tax ?? 0) * 0.01)))
  }


}


