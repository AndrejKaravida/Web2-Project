import { NgModule } from '@angular/core';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSliderModule } from '@angular/material/slider';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatTabsModule } from '@angular/material/tabs';
import { MatTableModule } from '@angular/material/table';
import { MatStepperModule } from '@angular/material/stepper';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDialogModule } from '@angular/material/dialog';
import { MatMenuModule } from '@angular/material/menu';


@NgModule({
    imports: [
      MatInputModule,
      MatButtonModule,
      MatCardModule,
      MatDividerModule,
      MatFormFieldModule,
      MatMenuModule,
      MatDatepickerModule,
      MatNativeDateModule,
      MatSelectModule,
      MatProgressSpinnerModule,
      MatSliderModule,
      MatPaginatorModule,
      MatAutocompleteModule,
      MatTabsModule,
      MatTableModule,
      MatStepperModule,
      MatCheckboxModule,
      MatDialogModule
    ],
    exports: [
      MatInputModule,
      MatMenuModule,
      MatButtonModule,
      MatCardModule,
      MatDividerModule,
      MatFormFieldModule,
      MatDatepickerModule,
      MatNativeDateModule,
      MatSelectModule,
      MatProgressSpinnerModule,
      MatSliderModule,
      MatPaginatorModule,
      MatAutocompleteModule,
      MatTabsModule,
      MatTableModule,
      MatStepperModule,
      MatCheckboxModule,
      MatDialogModule
    ]
  })
  export class MaterialModule {}