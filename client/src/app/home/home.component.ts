import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registerMode: boolean;
  users: any;

  constructor(private http:HttpClient) { }

  ngOnInit(): void {
    this.getUsers();
  }

  registerToggle(){
    this.registerMode = !this.registerMode;
  }
  
  
  getUsers(){
    this.http.get("https://localhost:5001/api/users").subscribe(response => {
      this.users = response;
    },error => {
      console.log(error);
    }
    )
  }

  cancelRegisterMode(event : boolean){
    this.registerMode = event;
  }
}
