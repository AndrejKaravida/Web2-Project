import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AvioCompany } from '../_models/aviocompany';
import { AvioService } from '../_services/avio.service';
import { Destination } from '../_models/destination';
import { AlertifyService } from '../_services/alertify.service';
import { Flight } from '../_models/flight';

@Component({
  selector: 'app-aviocompany-profile',
  templateUrl: './aviocompany-profile.component.html',
  styleUrls: ['./aviocompany-profile.component.css']
})


export class AviocompanyProfileComponent implements OnInit {

  company: AvioCompany;
  id: number;
  address: string;
  destinations: Destination[];
  averageGradeOfCompany: number;
  averageGradeOfFlies: number;
  income: number; // prihod
  startingDate = new Date();
  minPriceChosen: number;
  maxPriceChosen: number;
  startingLocation = '';
  returningLocation = '';

  constructor(private route: ActivatedRoute, private avioService: AvioService,
              private alertify: AlertifyService) { }


  ngOnInit() {
    this.route.params.subscribe(params => {
      this.id = params['id'];
      this.avioService.getAvioCompany(params.id).subscribe(res => {
        this.company = res;
        this.minPriceChosen = 0;
        this.maxPriceChosen = 1000;
      });
    });
  }

  buyTicket()
  {
    this.alertify.success('You have booked travel.');
  }

}
