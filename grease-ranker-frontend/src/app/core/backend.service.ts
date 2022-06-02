import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Product } from '../models/product';

@Injectable({
  providedIn: 'root'
})
export class BackendService {

  constructor(private http: HttpClient) { }

  private baseUrl = 'http://localhost:8080/';

  public getProducts(): Observable<Product[]> {
    return this.http.get<Product[]>(`${this.baseUrl}ranking`);
  }
}
