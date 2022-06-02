import { Component, OnInit } from '@angular/core';
import { BackendService } from 'src/app/core/backend.service';
import { Product } from 'src/app/models/product';

@Component({
  selector: 'app-mc-donalds',
  templateUrl: './mc-donalds.component.html',
  styleUrls: ['./mc-donalds.component.scss']
})
export class McDonaldsComponent implements OnInit {

  constructor(private backend: BackendService) { }

  products: Product[] = [];

  ngOnInit(): void {
    this.backend.getProducts().subscribe(x => {
      this.products = x;
    });
  }

}
