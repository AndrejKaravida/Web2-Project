import { Component, OnInit, Input } from '@angular/core';
import { Flight } from 'src/app/_models/_avioModels/flight';
import { AvioCompany } from 'src/app/_models/_avioModels/aviocompany';
import { AvioService } from 'src/app/_services/avio.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-discount-ticket-lists',
  templateUrl: './discount-ticket-lists.component.html',
  styleUrls: ['./discount-ticket-lists.component.css']
})
export class DiscountTicketListsComponent implements OnInit {
  flights: Flight[];
  company: AvioCompany;
  
  constructor(private avioService: AvioService, private activatedRoute: ActivatedRoute) { }

  ngOnInit() {
    this.activatedRoute.params.subscribe(res => {
      this.avioService.getDiscountedTickets(res.id).subscribe(response => {
        this.flights = response;
      });
    });

    this.activatedRoute.data.subscribe( data => {
      this.company = data['company'];
    });
  }

}
