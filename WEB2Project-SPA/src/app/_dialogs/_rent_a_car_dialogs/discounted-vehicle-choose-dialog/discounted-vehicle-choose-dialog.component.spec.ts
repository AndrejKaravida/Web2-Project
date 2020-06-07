/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { DiscountedVehicleChooseDialogComponent } from './discounted-vehicle-choose-dialog.component';

describe('DiscountedVehicleChooseDialogComponent', () => {
  let component: DiscountedVehicleChooseDialogComponent;
  let fixture: ComponentFixture<DiscountedVehicleChooseDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DiscountedVehicleChooseDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DiscountedVehicleChooseDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
