import { Component, inject, signal } from '@angular/core';
import { DataService } from '../data.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-table-bar',
  imports: [FormsModule],
  templateUrl: './table-bar.component.html',
  styleUrl: './table-bar.component.scss'
})
export class TableBarComponent {
  dataService = inject(DataService);
  filterTxt = signal<string>("");
  
}
