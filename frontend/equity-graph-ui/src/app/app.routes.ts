import { Routes } from '@angular/router';
import { HomeComponent } from './features/home/home.component';
import { CompanyDetailComponent } from './features/company-detail/company-detail.component';
import { PathFinderComponent } from './features/path-finder/path-finder.component';
import { NotFoundComponent } from './shared/components/not-found/not-found.component';

export const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
    pathMatch: 'full'
  },
  {
    path: 'company/:id',
    component: CompanyDetailComponent
  },
  {
    path: 'path-finder',
    component: PathFinderComponent
  },
  {
    path: '**',
    component: NotFoundComponent
  }
];
