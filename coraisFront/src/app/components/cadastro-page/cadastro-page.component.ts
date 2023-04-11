import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserService } from '../user.service';
import { catchError } from 'rxjs/internal/operators/catchError';
import { HttpErrorResponse } from '@angular/common/http';
import { throwError } from 'rxjs/internal/observable/throwError';

@Component({
  selector: 'app-cadastro-page',
  templateUrl: './cadastro-page.component.html',
  styleUrls: ['./cadastro-page.component.scss']
})
export class CadastroPageComponent {
  formulario!: FormGroup;
  duplicatedUsername: boolean = false;
  duplicatedEmail: boolean = false;

  constructor(
    private readonly formBuilder: FormBuilder,
    private readonly service: UserService
  ) {}
  
  ngOnInit(): void {
    this.formulario = this.formBuilder.group({
      Username: ['', [Validators.required, Validators.pattern(/(.|\s)*\S(.|\s)*/)]],
      Email: ['', [Validators.required, Validators.pattern(/(.|\s)*\S(.|\s)*/), Validators.email]],
      Password: ['', [Validators.required, Validators.pattern(/(.|\s)*\S(.|\s)*/)]],
      RePassword: ['', [Validators.required, Validators.pattern(/(.|\s)*\S(.|\s)*/)]],
    })
  }

  fazerCadastro(): void {
    const res = this.service.cadastro(this.formulario.value);
      res.pipe(
        catchError((error: HttpErrorResponse) => {
          if (error.status === 409) {
            const errors: string[] = error.error.errors;
            this.verificaEmailEUsernameDuplicados(errors);
          }
          return throwError(error);
        })
      )
      .subscribe((response: any) => {
        alert("Cadastro feito com sucesso")
      })
  }

  verificaEmailEUsernameDuplicados(errors: string[]) {
    if (errors.includes('DuplicateUserName')) {
      this.duplicatedUsername = true;
    } else {
      this.duplicatedUsername = false;
    }

    if (errors.includes('DuplicateEmail')) {
      this.duplicatedEmail = true;
    } else {
      this.duplicatedEmail = false;
    }
  }
}
