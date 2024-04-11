import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProductService } from '../../../services/product.service';
import { ProductDetail } from '../../../viewModels/product/product-detail.model';

@Component({
  selector: 'product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['product-details.component.css']
})
export class ProductDetailsComponent implements OnInit {

  productId!: string;
  productDetail!: ProductDetail;

  constructor(private productService: ProductService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.productId = params['id'];
      this.loadProductDetail();
    });
  }

  loadProductDetail(): void {
    this.productService.getProductDetails(this.productId).subscribe(
      productDetail => {
        this.productDetail = productDetail;
      }
    );
  }
}

