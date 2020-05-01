import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';

@Component({
  selector: 'app-value',
  templateUrl: './value.component.html',
  styleUrls: ['./value.component.css']
})
export class ValueComponent implements OnInit {
  results: any[];
  constructor(private httpClient: HttpClient) { }

  ngOnInit(): void {
    this.getValues();
  }

  getValues() {
    this.httpClient.get<any>('http://localhost:5000/api/values')
      .pipe (
        map(response => response),
        catchError(err => throwError(err))
      )
      .subscribe(result => this.results = result);
  }
}
