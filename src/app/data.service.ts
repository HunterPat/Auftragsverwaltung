import { computed, effect, inject, Injectable, OnInit, Signal, signal } from '@angular/core';
import { toSignal, rxResource } from '@angular/core/rxjs-interop'
import { OrderDto, OrdersService, StatusService } from './swagger';
import { MyOrderDto } from './shared/MyOrderDto';

@Injectable({
  providedIn: 'root'
})
export class DataService {
  filterOrders(filterTxt: string) {
    if(filterTxt.length === 0){
    this.refreshOrdersList();
      return;
    }
    this.orderService.ordersFilterFilterTxtGet(filterTxt).subscribe(x => {
      this.orders.set(x as MyOrderDto[]);
    });
  }

  constructor() {
    this.orderService.ordersPagePageNrGet(this.pageNumber() - 1).subscribe(x => {
      this.orders.set(x as MyOrderDto[]);
      this.orders().forEach(x => {
        x.brutto = parseFloat(x.brutto!.toFixed(2));
        x.netto = parseFloat(x.netto!.toFixed(2));
      });
    });
  }
  refreshOrdersList() {
    this.orders().splice(0, this.orders().length);
    this.orderService.ordersPagePageNrGet(this.pageNumber() - 1).subscribe(x => x.forEach(y => {
      y.brutto = parseFloat(y.brutto!.toFixed(2));
      y.netto = parseFloat(y.netto!.toFixed(2));
      this.orders().push(y as MyOrderDto)
    }));
  }
  orderService = inject(OrdersService);
  monthOverview = signal<boolean>(false);
  orders = signal<MyOrderDto[]>([]);
  statusList = toSignal(inject(StatusService).statusGet());
  pageNumber = signal<number>(1);

  GetSelectedOrders(): MyOrderDto[] {
    if (this.orders() === undefined) return [];
    return this.orders().filter(x => x.isSelected == true);
  }
  sortDirectionIsAsc = signal<boolean>(true);
  currentSortProperty = signal<string>(null!);
  GetOrderedOrders(orderProperty: string) {
    switch (orderProperty) {
      case "inputDate":
      case "deliveryDate":
      case "paymentDate":
      case "documentNr":
      case "brutto":
      case "netto":
      case "tax":
      case "customerName":
      case "bill":
      case "po":
      case "status":
      case "uid":
      case "billCreatedDate":
        // Use a reusable method for sorting
        this.currentSortProperty.set(orderProperty);
        this.sortOrders(orderProperty);
        break;
      default:
        console.warn(`Unknown property: ${orderProperty}`);
        break;
    }

  }
  sortOrders(property: keyof OrderDto) {
    this.orders.set(this.sortDirectionIsAsc() ? this.orders().orderBy(x => x[property]) : this.orders().orderByDescending(x => x[property]));
    this.sortDirectionIsAsc.set(!this.sortDirectionIsAsc());
  }
}