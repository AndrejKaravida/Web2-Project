import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NodataPipe } from './nodata.pipe';



@NgModule({
  declarations: [NodataPipe],
  exports: [
    NodataPipe
  ]
})
export class PipesModule { }
