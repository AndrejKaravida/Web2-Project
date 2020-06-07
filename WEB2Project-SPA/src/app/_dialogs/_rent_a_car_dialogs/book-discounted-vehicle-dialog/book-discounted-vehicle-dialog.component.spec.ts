/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { BookDiscountedVehicleDialogComponent } from './book-discounted-vehicle-dialog.component';

describe('BookDiscountedVehicleDialogComponent', () => {
  let component: BookDiscountedVehicleDialogComponent;
  let fixture: ComponentFixture<BookDiscountedVehicleDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BookDiscountedVehicleDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BookDiscountedVehicleDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
