import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AdminPanelComponent } from './admin-panel.component';
import { AuthGuard } from '../_guards/auth.guard';
import { SysAdminGuard } from '../_guards/sysadmin.guard';

const routes: Routes = [
  {
    path: '',
    component: AdminPanelComponent,
    canActivate : [AuthGuard, SysAdminGuard],
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
