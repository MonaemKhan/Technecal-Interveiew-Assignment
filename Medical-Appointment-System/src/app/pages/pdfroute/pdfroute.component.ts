import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { ApiService } from 'src/app/services/api.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-pdfroute',
  templateUrl: './pdfroute.component.html',
  styleUrls: ['./pdfroute.component.css']
})
export class PdfrouteComponent {
  pdfSrc: SafeResourceUrl | null = null;
  appointmentNo : any = "";
  emailAddress : string = "";

  constructor(private api: ApiService, private sanitizer: DomSanitizer, private route: ActivatedRoute) {}
  ngOnInit(): void {
    this.appointmentNo = this.route.snapshot.paramMap.get('appointNo');
    this.loadPdf();
    }
  loadPdf() {
    this.api.getReportFile(this.appointmentNo)
      .subscribe((res) => {
        const url = URL.createObjectURL(res);
        this.pdfSrc = this.sanitizer.bypassSecurityTrustResourceUrl(url);
      });
  }

  sendEmail(){
    this.api.sendEmial(this.emailAddress,this.appointmentNo).subscribe(res=>{
      alert("Email Succesfully Send")
      this.emailAddress = "";
    });
  }
}
