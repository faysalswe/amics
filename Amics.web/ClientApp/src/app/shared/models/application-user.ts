export class ApplicationUser {
  /**
   * Application User View Model.
   */
  constructor(
    public userId?: string,
    public userName?: string,
    public firstName?: string,
    public password?: string,
    public warehouse?: string,
    public lastName?: string,
    public email?: string,
    public userDataBase?: string,
    public buyer?: string,
    public salesPerson?: string,
    public webAccess?: string,
    public amicsUser?: string,
    public empList?: string,
    public invTrans?: string,
    public forgotPwdAns?: string
  ) {}

  isAuthenticated(): boolean {
    return !!this.userId;
  }
 
  isBasicUser(): boolean {
    return  true;
  }

  isAdmin(): boolean {
    return false;
  }

  isWebMaster(): boolean {
    return false;
  }
 
}
