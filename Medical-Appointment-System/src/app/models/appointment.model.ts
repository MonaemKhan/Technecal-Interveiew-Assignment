
export interface AppointmentModel {
  id?: number;
  appointmentNo: string;
  patientId: number;
  doctorId: number;
  appointmentDate: string;
  visitType: string;
  notes?: string;
  diagnosis?: string;
  isAppointmentVIsited?:string;
}
