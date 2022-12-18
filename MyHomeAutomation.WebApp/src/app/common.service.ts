import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CommonService {

  constructor() { }

  lostCommunication(date: Date): boolean{
    var now = new Date();
    var newDate = new Date(date);
    if ((now.getTime() - newDate.getTime()) > 10000){
      return  true;
    }
    return false;
  }
  
  getLostCommunicationClass(date: Date): string{
    if(this.lostCommunication(date))
      return "btn btn-danger";
    else 
      return "btn btn-success";
  }
}
