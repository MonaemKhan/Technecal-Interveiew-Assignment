import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { HeaderComponent } from './core/header/header.component';
import { AppointmentCreateComponent } from './pages/appointments/appointment-create/appointment-create.component';

import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { PrescriptionDetailsComponent } from './pages/Prescription/prescription-details/prescription-details.component';
import { PdfrouteComponent } from './pages/pdfroute/pdfroute.component';
import { SafeUrlPipe } from './pages/pdfroute/safe-url.pipe';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    AppointmentCreateComponent,
    PrescriptionDetailsComponent,
    PdfrouteComponent,
    SafeUrlPipe
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    CommonModule,
    ReactiveFormsModule,
    FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
