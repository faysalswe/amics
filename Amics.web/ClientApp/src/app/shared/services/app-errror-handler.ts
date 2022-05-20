import { ErrorHandler, Inject, Injector, Injectable } from "@angular/core";
import { HttpErrorResponse } from "@angular/common/http"; 
import { Router } from "@angular/router";
import { UserService } from "./user.service";
import notify from "devextreme/ui/notify";

@Injectable({
  providedIn: "root",
})
export class AppErrorHandler extends ErrorHandler {
  constructor(@Inject(Injector) private injector: Injector) {
    super();
  }
 

  private get userService(): UserService {
    return this.injector.get(UserService);
  }
  private get router(): Router {
    return this.injector.get(Router);
  }
  public handleError(error: any): void {
    if (error instanceof HttpErrorResponse) {
      if (error.status === 0) {
        this.notifyError(
          `Developers have been notified and will resolve this issue soon.`,
          `System Error`
        );
        this.userService.getUser().then(() => {
          this.router.navigateByUrl("/home");
        });
      } else {
        const msg = error.error ? error.error.message : error.message;
        this.notifyError(msg || "error");
      }
    } else if (error instanceof TypeError) {
      this.notifyError(error.message, "TypeScript Type error.");
    } else if (error instanceof Error) {
      this.notifyError(error.message);
    } else {
      this.notifyError("Something unexpected happened...");
    }
    super.handleError(error);
  }

  private notifyError(msg: string, title?: string) {
    notify(msg, 'error', 5000);
  }
}
