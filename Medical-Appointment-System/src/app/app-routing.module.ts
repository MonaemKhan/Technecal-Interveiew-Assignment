import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppointmentCreateComponent } from './pages/appointments/appointment-create/appointment-create.component';
import { PrescriptionDetailsComponent } from './pages/Prescription/prescription-details/prescription-details.component';
import { PdfrouteComponent } from './pages/pdfroute/pdfroute.component';

const routes: Routes = [
  { path: '', redirectTo: 'appointments', pathMatch: 'full' },
  { path: 'appointments', component: AppointmentCreateComponent },
  { path: 'prescription', component: PrescriptionDetailsComponent },
  { path: 'report/:appointNo', component: PdfrouteComponent },
  { path: '**', redirectTo: 'appointments' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
