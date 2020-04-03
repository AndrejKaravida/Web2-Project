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
  chosenStarting = '';
  chosenArrival: string;
  flights = [];

  constructor(private route: ActivatedRoute, private avioService: AvioService,
              private alertify: AlertifyService) { }


  ngOnInit() {
    for(let i = 0; i < 5; i++)
    {
      this.flights.push("1");
    }


    this.route.params.subscribe(params => {
      this.id = params['id'];
      this.avioService.getAvioCompany(params.id).subscribe(res => {
        this.company = res;
        console.log(res);
      });
    });

    this.loadDestinations();
  }

  loadDestinations() {
    this.avioService.getAllDestinations().subscribe(response => { 
     this.destinations = response;
    });
  }

  buyTicket()
  {
    this.alertify.success('You have booked travel.');
  }

}
