import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'safeFormat',
})
export class SafeFormatPipe implements PipeTransform {
  transform(value: any, args?: any): any {
    if (typeof value === 'number') {
      return Number(value.toFixed(1));
    } else {
      return value;
    }
  }
}
