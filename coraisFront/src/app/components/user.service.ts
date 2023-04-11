import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LoginUserDto } from './dtos/loginUserDto';
import { TokenMessage } from './dtos/tokenMessage';
import { Observable } from 'rxjs';
import { CreateUserDto } from './dtos/createUserDto';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private readonly api = 'http://localhost:8080';

  constructor(
    private readonly http: HttpClient
  ) {}

  login(dto: LoginUserDto): Observable<TokenMessage> {
    return this.http.post<TokenMessage>(`${this.api}/login`, dto);
  }

  cadastro(dto: CreateUserDto) {
    return this.http.post(`${this.api}/cadastro`, dto);
  }
}
