import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  Router,
  RouterStateSnapshot,
  UrlTree,
} from '@angular/router';
import { Observable } from 'rxjs';
import { DataStoreService } from '../services/data-store.service';

@Injectable({
  providedIn: 'root',
})
export class AuthGuardGuard implements CanActivate {
  /**
   *
   */
  constructor(
    private dataStoreService: DataStoreService,
    private router: Router
  ) {}
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> {
    return this.dataStoreService.accessToken$.pipe(
      map((token) => {
        if (token) return true;
        this.router.navigate(['/']);
        return false;
      })
    );
  }
}
