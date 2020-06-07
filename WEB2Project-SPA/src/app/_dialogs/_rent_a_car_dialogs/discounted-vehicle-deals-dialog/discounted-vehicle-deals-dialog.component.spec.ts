/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { DiscountedVehicleDealsDialogComponent } from './discounted-vehicle-deals-dialog.component';

describe('DiscountedVehicleDealsDialogComponent', () => {
  let component: DiscountedVehicleDealsDialogComponent;
  let fixture: ComponentFixture<DiscountedVehicleDealsDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DiscountedVehicleDealsDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DiscountedVehicleDealsDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
