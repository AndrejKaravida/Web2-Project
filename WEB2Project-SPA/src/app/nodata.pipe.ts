import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'nodata'
})
export class NodataPipe implements PipeTransform {

  transform(value: any, args?: any): any {
    if (value === 0) {
      return 'No data';
    }
    const result = (value * 10).toFixed();
    return result.toString() + ' %';
  }

}
