import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BurgerKingComponent } from './pages/burger-king/burger-king.component';
import { McDonaldsComponent } from './pages/mc-donalds/mc-donalds.component';

const routes: Routes = [
  { path: 'mc-donalds', component: McDonaldsComponent },
  { path: 'burger-king', component: BurgerKingComponent },
  { path: '', redirectTo: '/mc-donalds', pathMatch: 'full' },
  { path: '**', redirectTo: '/mc-donalds' },
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, { useHash: true })
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
