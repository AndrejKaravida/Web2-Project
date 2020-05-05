/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { CompanyAddSuccessfullDialogComponent } from './company-add-successfull-dialog.component';

describe('CompanyAddSuccessfullDialogComponent', () => {
  let component: CompanyAddSuccessfullDialogComponent;
  let fixture: ComponentFixture<CompanyAddSuccessfullDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CompanyAddSuccessfullDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CompanyAddSuccessfullDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
