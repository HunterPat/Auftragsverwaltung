import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ErrorComponent } from './error/error.component';
import { YearOverviewComponent } from './year-overview/year-overview.component';
import { MonthOverviewComponent } from './month-overview/month-overview.component';

export const routes: Routes = [
  //{ path: '', loadComponent: () => import('./test-pages/test-general/test-general.component').then(x => x.TestGeneralComponent) },
  { path: '', component: HomeComponent, title: "HomeComponent" },
  { path: 'yearOverview', component: YearOverviewComponent, title: "Jahresübersicht" },
  { path: 'monthOverview', component: MonthOverviewComponent, title: "Monatsübersicht" },
  // { path: '**', component: ErrorComponent, title: "404 Not found" },
  { path: '**', redirectTo: '' },
];
