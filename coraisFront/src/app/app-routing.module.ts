import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginPageComponent } from './components/login-page/login-page.component';
import { CadastroPageComponent } from './components/cadastro-page/cadastro-page.component';

const routes: Routes = [
  {
    path: '',
    redirectTo: '/loginPage',
    pathMatch: 'full'
  },
  {
    path: 'loginPage',
    component: LoginPageComponent
  },
  {
    path: 'cadastroPage',
    component: CadastroPageComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
