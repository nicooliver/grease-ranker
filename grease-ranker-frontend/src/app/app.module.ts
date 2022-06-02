import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { McDonaldsComponent } from './pages/mc-donalds/mc-donalds.component';
import { BurgerKingComponent } from './pages/burger-king/burger-king.component';
import { CoreModule } from './core/core.module';
import { ProductCardComponent } from './shared/product-card/product-card.component';
import { SharedModule } from './shared/shared.module';

@NgModule({
  declarations: [
    AppComponent,
    McDonaldsComponent,
    BurgerKingComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    CoreModule,
    SharedModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
