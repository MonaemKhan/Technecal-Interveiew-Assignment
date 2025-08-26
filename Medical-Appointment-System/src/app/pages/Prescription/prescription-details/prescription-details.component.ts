import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, FormArray, Validators } from "@angular/forms";
import { ApiService } from "../../../services/api.service";
import { MedecineModel } from "src/app/models/medecine.model";
import { AppointmnetDropdownMOdel } from "src/app/models/apointment.dropdown.model";

@Component({
  selector: "app-prescription-details",
  templateUrl: "./prescription-details.component.html",
  styleUrls: ["./prescription-details.component.css"],
})
export class PrescriptionDetailsComponent implements OnInit {
  prescriptionForm!: FormGroup;
  medicines: MedecineModel[] = [];
  appointmentList: any[] = [];
  apointmentId: string = "";

  constructor(
    private fb: FormBuilder,
    private api: ApiService,
  ) {}

  ngOnInit(): void {
    this.prescriptionForm = this.fb.group({
      apointmentId: [""],
      prescriptions: this.fb.array([]),
    });

    this.loadAppointmnet();
    this.loadMedicines();

    // Start with one row
    this.prescriptions.push(this.newPrescription());
  }

  get prescriptions(): FormArray {
    return this.prescriptionForm.get("prescriptions") as FormArray;
  }

  loadAppointmnet() {
    this.api
      .getAppointmentDropDown()
      .subscribe((res: any[]) => {
        this.appointmentList = res;
      });
  }

  loadMedicines() {
    this.api.getAllMedecine().subscribe((res: MedecineModel[]) => {
      this.medicines = res;
    });
  }

  newPrescription(): FormGroup {
    return this.fb.group({
      medicineId: [null, Validators.required],
      dosage: ["", Validators.required],
      startDate: ["", Validators.required],
      endDate: ["", Validators.required],
      notes: ["", Validators.required],
      appointmentNo: [this.prescriptionForm.get("apointmentId")?.value],
    });
  }

  addPrescription() {
    let allValid = true;
    this.prescriptions.controls.forEach((control, index) => {
      if (control.invalid) {
        allValid = false;
        control.markAllAsTouched();
      }
    });

    if (!allValid) {
      alert(
        "Please fill all fields in current prescriptions before adding a new one.",
      );
      return;
    }
    this.prescriptions.push(this.newPrescription());
  }

  removePrescription(index: number) {
    this.prescriptions.removeAt(index);
  }

  save() {
    if (
      this.prescriptionForm.value.apointmentId === null ||
      this.prescriptionForm.value.apointmentId === ""
    ) {
      alert(`Select Appointment No. Requried`);
      return;
    }

    let allValid = true;
    this.prescriptions.controls.forEach((control, index) => {
      if (control.invalid) {
        allValid = false;
        console.log(control);
        control.markAllAsTouched();
      }
    });

    if (!allValid) {
      alert(
        "Please fill all fields in current prescriptions before adding a new one.",
      );
      return;
    }

    this.prescriptions.value.forEach((value: any, index: number) => {
      value.appointmentNo = this.prescriptionForm.value.apointmentId;
    });

    this.api
      .SavePrescription(this.prescriptionForm.value.prescriptions)
      .subscribe((res) => {
        alert("Prescription saved successfully!");
        this.prescriptionForm.reset();
        this.prescriptions.clear();

        this.prescriptions.push(this.newPrescription());

        this.loadAppointmnet();
      });
  }
}
