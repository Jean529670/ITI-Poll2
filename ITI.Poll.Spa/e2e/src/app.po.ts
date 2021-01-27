import { browser, by, element } from 'protractor';

export class AppPage {
  async navigateTo(): Promise<unknown> {
    return browser.get(browser.baseUrl);
  }

  async HomeLinkText(): Promise<string> {
    return element.all(by.css('app-root .app__nav ul li')).first().getText();
  }

  async navigateToSignUp(): Promise<unknown> {
    return browser.get(`${browser.baseUrl}signup`);
  }

  async fillSignUpForm(email: string, nickname: string, password: string): Promise<void> {
    const inputElements = element.all(by.tagName('input'));
    const emailElement = inputElements.get(0);
    const nicknameElement = inputElements.get(1);
    const passwordElement = inputElements.get(2);

    await emailElement.sendKeys(email);
    await nicknameElement.sendKeys(nickname);
    await passwordElement.sendKeys(password);
  }

  async signUp(): Promise<unknown> {
    const signUpElement = element(by.tagName('button'));
    await signUpElement.click();
    return browser.waitForAngular();
  }

  async email(): Promise<string> {
    return element(by.css('app-root .app__nav ul li.last')).getText();
  }
}
