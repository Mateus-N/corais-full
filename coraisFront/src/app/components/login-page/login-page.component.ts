import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserService } from '../user.service';
import { catchError } from 'rxjs/internal/operators/catchError';
import { throwError } from 'rxjs/internal/observable/throwError';
import { TokenMessage } from '../dtos/tokenMessage';

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.scss']
})
export class LoginPageComponent {
  formulario!: FormGroup;
  loginUnauthorized: boolean = false;

  constructor(
    private readonly formBuilder: FormBuilder,
    private readonly service: UserService
  ) {}

  ngOnInit(): void {
    this.formulario = this.formBuilder.group({
      Username: ['', [Validators.required, Validators.pattern(/(.|\s)*\S(.|\s)*/)]],
      Password: ['', [Validators.required]]
    })
  }

  fazerLogin(): void {
    if (this.formulario.valid) {
      const res = this.service.login(this.formulario.value);
      res.pipe(
        catchError((error: HttpErrorResponse) => {
          if (error.status === 401) {
            this.loginUnauthorized = true;
          }
          return throwError(error);
        })
      )
      .subscribe((response: TokenMessage) => {
        this.loginUnauthorized = false;
        alert(response.message);
      })
    }
  }
}
