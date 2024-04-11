import { Component, Inject } from '@angular/core';
import { ProductList } from '../../viewModels/product/product-list.model';
import { ProductService } from '../../services/product.service';
import { Router } from '@angular/router';

@Component({
  selector: 'product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['product-list.component.css']
})
export class ProductListComponent {
  productList!: ProductList;
  
  constructor(private productService: ProductService, private router: Router) {  }

  ngOnInit(): void {
    this.loadProductList();
  }

  loadProductList(): void {
    this.productService.getProductList().subscribe(
      productList => {
        this.productList = productList;
      }
    );
  }

  navigateToDetails(productId: string): void {
    this.router.navigate(['/products', productId]);
  }
}

