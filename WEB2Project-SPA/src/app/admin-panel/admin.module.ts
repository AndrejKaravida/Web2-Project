import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminRoutingModule } from './admin-routing.module';
import { AdminPanelComponent } from './admin-panel.component';
import { MaterialModule } from '../material.module';
import { PipesModule } from '../pipes.module';
import { AddNewCompanyDialogComponent } from '../_dialogs/_adminpanel_dialogs/add-new-company-dialog/add-new-company-dialog.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    AdminPanelComponent,
    AddNewCompanyDialogComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    PipesModule,
    ReactiveFormsModule,
    MaterialModule,
    AdminRoutingModule
  ],
  entryComponents: [
    AddNewCompanyDialogComponent
  ]
})
export class AdminModule { }
