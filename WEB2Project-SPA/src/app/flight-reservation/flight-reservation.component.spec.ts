/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { FlightReservationComponent } from './flight-reservation.component';

describe('FlightReservationComponent', () => {
  let component: FlightReservationComponent;
  let fixture: ComponentFixture<FlightReservationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FlightReservationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FlightReservationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
