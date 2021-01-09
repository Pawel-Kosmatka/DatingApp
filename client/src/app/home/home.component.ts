import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  isRegisterMode = false;

  constructor() { }

  ngOnInit(): void {
  }

  registerToggle() {
    this.isRegisterMode = !this.isRegisterMode;
  }

  cancelRegisterMode(event: boolean) {
    this.isRegisterMode = event;
  }
}