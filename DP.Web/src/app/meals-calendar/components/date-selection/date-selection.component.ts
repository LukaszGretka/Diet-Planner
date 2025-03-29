import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { NgbDate } from '@ng-bootstrap/ng-bootstrap';
import { DatePickerSelection } from '../../models/date-picker-selection';

@Component({
  selector: 'app-date-selection',
  templateUrl: './date-selection.component.html',
  styleUrl: './date-selection.component.scss',
})
export class DateSelectionComponent implements OnInit {
  @Output()
  public selectedDate = new EventEmitter<Date>();

  public dateModel: DatePickerSelection;

  public ngOnInit(): void {
    const dateNow = new Date();

    this.dateModel = {
      day: dateNow.getDate(),
      month: dateNow.getMonth() + 1, // getMonth method is off by 1. (0-11)
      year: dateNow.getFullYear(),
    };

    this.selectedDate.emit(dateNow);
  }

  public onDateSelection(ngbDate: NgbDate): void {
    const selectedDate = new Date();
    selectedDate.setFullYear(ngbDate.year);
    selectedDate.setMonth(ngbDate.month - 1);
    selectedDate.setDate(ngbDate.day);
    this.selectedDate.emit(selectedDate);
  }
}
