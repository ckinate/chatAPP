import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  model:any={}
  loggIn:boolean;

  constructor(private accountService: AccountService) { }

  ngOnInit(): void {
  }
 login (){
   this.accountService.login(this.model).subscribe(result=>{
     console.log(result);
     this.loggIn = true;
   });
   
   
 }
}
