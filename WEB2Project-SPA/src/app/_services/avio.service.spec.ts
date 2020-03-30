/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { AvioService } from './avio.service';

describe('Service: Avio', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AvioService]
    });
  });

  it('should ...', inject([AvioService], (service: AvioService) => {
    expect(service).toBeTruthy();
  }));
});
