import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { PatientModel } from '../models/patient.model';
import { DoctorModel } from '../models/doctor.model';
import { AppointmentModel } from '../models/appointment.model';
import { MedecineModel } from '../models/medecine.model';
import { AppointmnetDropdownMOdel } from '../models/apointment.dropdown.model';

@Injectable({ providedIn: 'root' })
export class ApiService {
  private base = environment.apiBaseUrl;

  constructor(private http: HttpClient) {}

  // Patients
  getPatients(): Observable<PatientModel[]> {
    return this.http.get<PatientModel[]>(`${this.base}/Patient`).pipe(catchError(this.handleError))
  }

  getPatient(id : number): Observable<PatientModel> {
    return this.http.get<PatientModel>(`${this.base}/Patient/${id}`).pipe(catchError(this.handleError))
  }

  // Doctors
  getDoctors(): Observable<DoctorModel[]> {
    return this.http.get<DoctorModel[]>(`${this.base}/Doctor`).pipe(catchError(this.handleError))
  }
  getDoctor(id : number): Observable<DoctorModel> {
    return this.http.get<DoctorModel>(`${this.base}/Doctor/${id}`).pipe(catchError(this.handleError))
  }

  // Appointments
  getAppointments(pageNumber :number, pagesige : number,searchText : string,arrowDir:string,sortValue:string,visitFilter:string,appstatus:string): Observable<AppointmentModel[]> {
    return this.http.get<AppointmentModel[]>(`${this.base}/Apointment?pageNumber=${pageNumber}&pageSize=${pagesige}
      &searchText=${searchText}&arrowDir=${arrowDir}&sortValue=${sortValue}&visitFilter=${visitFilter}&appStatus=${appstatus}`).pipe(
      catchError(this.handleError)
    );
  }

  createAppointment(payload: AppointmentModel): Observable<AppointmentModel> {
    return this.http.post<AppointmentModel>(`${this.base}/Apointment`, payload).pipe(
      catchError(this.handleError)
    );
  }

  updateAppointment(payload: AppointmentModel): Observable<AppointmentModel> {
    return this.http.put<AppointmentModel>(`${this.base}/Apointment/${payload.id}`, payload).pipe(
      catchError(this.handleError)
    );
  }

  DeleteAppointment(payload: AppointmentModel): Observable<AppointmentModel> {
    return this.http.delete<AppointmentModel>(`${this.base}/Apointment/${payload.id}`).pipe(
      catchError(this.handleError)
    );
  }

  getAppointmentDropDown(){
    return this.http.get<any[]>(`${this.base}/Apointment/dropdown`).pipe(
      catchError(this.handleError)
    );
  }

  getAllMedecine(){
    return this.http.get<MedecineModel[]>(`${this.base}/Medicine`).pipe(
      catchError(this.handleError)
    );
  }

  SavePrescription(data : any[]){
    return this.http.post<any[]>(`${this.base}/Prescription`,data).pipe(
      catchError(this.handleError)
    );
  }

  getReportFile(appointmenrid : string){
    return this.http.get(`${this.base}/Download/file?appointmentId=${appointmenrid}`,{ responseType: 'blob' }).pipe(
      catchError(this.handleError)
    );
  }

  sendEmial(emailId:string,appointId:string){
    return this.http.get<any>(`${this.base}/Email/send?EmailAddress=${emailId}&appointmentNo=${appointId}`).pipe(
      catchError(this.handleError)
    );
  }

  // common error handler
  private handleError(err: any) {
    console.error('API error:', err);
    return throwError(() => err);
  }
}
