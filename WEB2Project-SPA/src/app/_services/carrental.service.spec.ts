/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { CarrentalService } from './carrental.service';

describe('Service: Carrental', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [CarrentalService]
    });
  });

  it('should ...', inject([CarrentalService], (service: CarrentalService) => {
    expect(service).toBeTruthy();
  }));
});
