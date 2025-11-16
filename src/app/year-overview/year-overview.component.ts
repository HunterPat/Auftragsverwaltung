import { Component, inject, signal } from '@angular/core';
import { DataService } from '../data.service';
import { YearInfoDto } from '../swagger';
import { FormsModule } from '@angular/forms';
import { rxResource } from '@angular/core/rxjs-interop';
import { JsonPipe } from '@angular/common';

@Component({
  selector: 'app-year-overview',
  imports: [FormsModule],
  templateUrl: './year-overview.component.html',
  styleUrl: './year-overview.component.scss'
})
export class YearOverviewComponent {
  dataService = inject(DataService);
  selectedYear = signal<number>(new Date().getFullYear());
  yearInfo = rxResource({
    request: this.selectedYear,
    loader: () => this.dataService.orderService.ordersYearInfoYearGet(this.selectedYear().toString()),

  })

  GetLast10Years(): number[] {
    let date = new Date();
    let years: number[] = [];
    for (let index = 0; index < 10; index++) {
      years.push(date.getFullYear() - index);
    }
    return years;
  }
}
