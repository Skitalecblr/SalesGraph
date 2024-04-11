import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormBuilder, FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { ProductListComponent } from './product/product-list.component';
import { ProductDetailsComponent } from './product/details/product-details.component';
import { SaleListComponent } from './product/details/sales/sale-list.component';
import { PaginationComponent } from './shared/pagination/pagination.component';
import { SalesGraphComponent } from './graph/sales-graph.component';
import { DatePipe } from '@angular/common';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    ProductListComponent,
    ProductDetailsComponent,
    SaleListComponent,
    SalesGraphComponent,
    PaginationComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'products', component: ProductListComponent },
      { path: 'products/:id', component: ProductDetailsComponent },
      { path: 'salesGraph', component: SalesGraphComponent },
    ])
  ],
  providers: [FormBuilder, DatePipe],
  bootstrap: [AppComponent]
})
export class AppModule { }
