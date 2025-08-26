import { Component, OnInit } from "@angular/core";
import { FormBuilder, Validators } from "@angular/forms";
import { ApiService } from "src/app/services/api.service";
import { PatientModel } from "src/app/models/patient.model";
import { DoctorModel } from "src/app/models/doctor.model";
import { AppointmentModel } from "src/app/models/appointment.model";
import { ECDH } from "crypto";
import { error } from "console";

@Component({
  selector: "app-appointment-create",
  templateUrl: "./appointment-create.component.html",
  styleUrls: ["./appointment-create.component.css"],
})
export class AppointmentCreateComponent implements OnInit {
  patients!: PatientModel[];
  doctors!: DoctorModel[];
  appointments: AppointmentModel[] = [];
  isModalOpen = false;
  isDeteleBtnShow = false;
  buttonName: string = "Save Appointment";
  searchText: string = "";

  upArrow: string = "↑";
  downArrow: string = "↓";

  pataintArrow: string = "";
  DoctorArrow: string = "";
  DateArrow: string = "";
  apointmentArrow: string = "";
  visitArrow: string = "";
  appStatusArrow: string = "";

  arrowDir: string = "";
  sortValue: string = "";

  // Pagination
  page = 1;
  pageSize = 5;
  totalPages = 1;

  pageSizeOptions: number[] = [5, 10, 20];

  visitFilter: string = "";
  appStatus: string = "";

  form = this.fb.group({
    id: [0],
    appointmentNo: [{ value: "", disabled: true }, [Validators.required]],
    patientId: [null as number | null, [Validators.required]],
    doctorId: [null as number | null, [Validators.required]],
    appointmentDate: ["", [Validators.required]],
    visitType: ["First", [Validators.required]],
    notes: [""],
    diagnosis: [""],
  });

  constructor(
    private fb: FormBuilder,
    private api: ApiService,
  ) {}

  ngOnInit(): void {
    this.api.getPatients().subscribe((data) => (this.patients = data));
    this.api.getDoctors().subscribe((data) => (this.doctors = data));

    this.form.patchValue({ appointmentNo: "[AUTO]" });
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const raw = this.form.getRawValue();
    const payload: AppointmentModel = {
      id: raw.id!,
      appointmentNo: raw.appointmentNo!,
      patientId: raw.patientId!,
      doctorId: raw.doctorId!,
      appointmentDate: raw.appointmentDate!,
      visitType: raw.visitType!,
      notes: raw.notes || "",
      diagnosis: raw.diagnosis || "",
    };
    console.log(payload.id);
    if (payload.id === 0) {
      this.api.createAppointment(payload).subscribe((res) => {
        alert(`Appointment created! \nAppointment No. - #${res.appointmentNo}`);
        this.form.reset({ appointmentNo: "[AUTO]", visitType: "First", id: 0 });
      });
    } else {
      this.api.updateAppointment(payload).subscribe((res) => {
        alert(
          `Appointment Updated! \nUpdated Apoointment No. - #${res.appointmentNo}`,
        );
        this.form.reset({ appointmentNo: "[AUTO]", visitType: "First", id: 0 });
        this.buttonName = "Save Appointment";
        this.isDeteleBtnShow = false;
      });
    }
  }

  SearchInput() {
    this.loadAppointments();
  }

  openModal() {
    this.isModalOpen = true;
    this.loadAppointments();
  }

  closeModal() {
    this.isModalOpen = false;
    this.pataintArrow = "";
    this.DoctorArrow = "";
    this.DateArrow = "";
    this.apointmentArrow = "";
    this.visitArrow = "";
    this.appStatusArrow = "";
    this.arrowDir = "";
    this.sortValue = "";
  }

  loadAppointments() {
    this.api
      .getAppointments(
        this.page,
        this.pageSize,
        this.searchText,
        this.arrowDir,
        this.sortValue,
        this.visitFilter,
        this.appStatus,
      )
      .subscribe((res: any) => {
        this.appointments = res.items;
        this.totalPages = res.totalPages;
        this.page = res.pageNumber;
      });
  }

  nextPage() {
    if (this.page < this.totalPages) {
      this.page++;
      this.loadAppointments();
    }
  }

  prevPage() {
    if (this.page > 1) {
      this.page--;
      this.loadAppointments();
    }
  }

  selectAppointment(a: AppointmentModel) {
    this.buttonName = "Update Appointment";
    this.isDeteleBtnShow = true;
    this.form.patchValue({
      id: a.id,
      appointmentNo: a.appointmentNo,
      patientId: a.patientId,
      doctorId: a.doctorId,
      appointmentDate: a.appointmentDate,
      visitType: a.visitType,
      notes: a.notes,
      diagnosis: a.diagnosis,
    });
    this.closeModal();
  }

  selectDelete(a: AppointmentModel) {
    this.api.DeleteAppointment(a).subscribe(
      (res) => {
        alert(`Appointment Deleted! \nAppointment No. - #${res.appointmentNo}`);
        this.form.reset({ appointmentNo: "[AUTO]", visitType: "First", id: 0 });
        this.buttonName = "Save Appointment";
        this.isDeteleBtnShow = false;
        this.openModal;
      },
      (error) => {
        console.log("error Happend");
      },
    );
  }

  getPatientName(id: number) {
    return this.patients.find((p) => p.id === id)?.name || "";
  }

  getDoctorName(id: number) {
    return this.doctors.find((p) => p.id === id)?.name || "";
  }

  DeleteAppointment() {
    const raw = this.form.getRawValue();
    const payload: AppointmentModel = {
      id: raw.id!,
      appointmentNo: raw.appointmentNo!,
      patientId: raw.patientId!,
      doctorId: raw.doctorId!,
      appointmentDate: raw.appointmentDate!,
      visitType: raw.visitType!,
      notes: raw.notes || "",
      diagnosis: raw.diagnosis || "",
    };

    this.api.DeleteAppointment(payload).subscribe((res) => {
      alert(`Appointment Deleted! \nAppointment No. - #${res.appointmentNo}`);
      this.form.reset({ appointmentNo: "[AUTO]", visitType: "First", id: 0 });
      this.buttonName = "Save Appointment";
      this.isDeteleBtnShow = false;
    });
  }

  pataintClick() {
    this.DoctorArrow = "";
    this.DateArrow = "";
    this.apointmentArrow = "";
    this.visitArrow = "";
    this.appStatusArrow = "";
    this.sortValue = "pat";
    if (this.page > 1) {
      this.page = 1;
    }
    if (this.pataintArrow === "") {
      this.pataintArrow = this.upArrow;
    } else if (this.pataintArrow === this.upArrow) {
      this.pataintArrow = this.downArrow;
    } else {
      this.pataintArrow = "";
      this.sortValue = "";
    }
    this.arrowDir = this.pataintArrow;
    this.loadAppointments();
  }

  DoctorClick() {
    this.pataintArrow = "";
    this.DateArrow = "";
    this.apointmentArrow = "";
    this.visitArrow = "";
    this.appStatusArrow = "";
    this.sortValue = "doc";
    if (this.page > 1) {
      this.page = 1;
    }
    if (this.DoctorArrow === "") {
      this.DoctorArrow = this.upArrow;
    } else if (this.DoctorArrow === this.upArrow) {
      this.DoctorArrow = this.downArrow;
    } else {
      this.DoctorArrow = "";
      this.sortValue = "";
    }
    this.arrowDir = this.DoctorArrow;
    this.loadAppointments();
  }

  DateClick() {
    this.pataintArrow = "";
    this.DoctorArrow = "";
    this.apointmentArrow = "";
    this.visitArrow = "";
    this.appStatusArrow = "";
    this.sortValue = "date";
    if (this.page > 1) {
      this.page = 1;
    }
    if (this.DateArrow === "") {
      this.DateArrow = this.upArrow;
    } else if (this.DateArrow === this.upArrow) {
      this.DateArrow = this.downArrow;
    } else {
      this.DateArrow = "";
      this.sortValue = "";
    }
    this.arrowDir = this.DateArrow;
    this.loadAppointments();
  }

  appointmentClick() {
    this.pataintArrow = "";
    this.DoctorArrow = "";
    this.DateArrow = "";
    this.visitArrow = "";
    this.appStatusArrow = "";
    this.sortValue = "apt";
    if (this.page > 1) {
      this.page = 1;
    }
    if (this.apointmentArrow === "") {
      this.apointmentArrow = this.upArrow;
    } else if (this.apointmentArrow === this.upArrow) {
      this.apointmentArrow = this.downArrow;
    } else {
      this.apointmentArrow = "";
      this.sortValue = "";
    }
    this.arrowDir = this.apointmentArrow;
    this.loadAppointments();
  }

  VisitClick() {
    this.pataintArrow = "";
    this.DoctorArrow = "";
    this.DateArrow = "";
    this.apointmentArrow = "";
    this.appStatusArrow = "";
    this.sortValue = "vis";
    if (this.page > 1) {
      this.page = 1;
    }
    if (this.visitArrow === "") {
      this.visitArrow = this.upArrow;
    } else if (this.visitArrow === this.upArrow) {
      this.visitArrow = this.downArrow;
    } else {
      this.visitArrow = "";
      this.sortValue = "";
    }
    this.arrowDir = this.visitArrow;
    this.loadAppointments();
  }

  AppStatusClick() {
    this.pataintArrow = "";
    this.DoctorArrow = "";
    this.DateArrow = "";
    this.apointmentArrow = "";
    this.visitArrow = "";
    this.sortValue = "dis";
    if (this.page > 1) {
      this.page = 1;
    }
    if (this.appStatusArrow === "") {
      this.appStatusArrow = this.upArrow;
    } else if (this.appStatusArrow === this.upArrow) {
      this.appStatusArrow = this.downArrow;
    } else {
      this.appStatusArrow = "";
      this.sortValue = "";
    }
    this.arrowDir = this.appStatusArrow;
    this.loadAppointments();
  }

  onChangeEvent(event: any) {
    this.loadAppointments();
  }
}
