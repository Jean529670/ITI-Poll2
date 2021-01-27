import { ComponentFixture, TestBed } from "@angular/core/testing"
import { PollComponent } from "./poll.component"

describe('PollComponent', () => {
  let fixture: ComponentFixture<PollComponent>;
  let component: PollComponent;

  beforeEach(async () => {
    await TestBed.configureTestingModule({ declarations: [ PollComponent ] })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PollComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });

  it('should display a poll', () => {
    component.poll = {
      pollId: 3712,
      question: 'Question?',
      answers: [
        { text: 'P1', votesCount: 0 },
        { text: 'P2', votesCount: 0 }
      ],
      guestCount: 4
    };
    fixture.detectChanges();

    const element: HTMLElement = fixture.nativeElement;
    const title = element.querySelector('h1');
    expect(title.innerText).toBe('Question?');
  });
})