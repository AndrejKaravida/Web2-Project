/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { RentacaroptiondialogComponent } from './rentacaroptiondialog.component';

describe('RentacaroptiondialogComponent', () => {
  let component: RentacaroptiondialogComponent;
  let fixture: ComponentFixture<RentacaroptiondialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RentacaroptiondialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RentacaroptiondialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
