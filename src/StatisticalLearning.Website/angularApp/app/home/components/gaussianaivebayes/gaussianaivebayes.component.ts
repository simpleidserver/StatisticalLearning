import { Component, OnInit } from '@angular/core';
import * as fromAppState from '@app/stores/appstate';
import * as fromStatisticActions from '@app/stores/statistic/statistic-actions';
import { select, Store } from '@ngrx/store';
import { GaussianNaiveBayesResult } from '../../../stores/statistic/models/gaussian-naivebayes.result';

@Component({
    selector: 'gaussianaivebayes-component',
    templateUrl: './gaussianaivebayes.component.html',
    styleUrls: ['./gaussianaivebayes.component.scss']
})
export class GaussianNaiveBayesComponent implements OnInit {
    gaussianNaiveBayesResult: GaussianNaiveBayesResult;
    inputs: number[][] = [
        [0, 1],
        [0, 2],
        [0, 1],
        [1, 2],
        [0, 2],
        [0, 2],
        [1, 1],
        [0, 1],
        [1, 1]    
    ];
    outputs: number[] = [0, 0, 0, 1, 1, 1, 2, 2, 2];
    predict: number[][] = [[0,1]];

    constructor(private store: Store<fromAppState.AppState>) { }

    ngOnInit(): void {
        this.store.pipe(select(fromAppState.selectGaussianNaiveBayesResult)).subscribe((gaussianNaiveBayes: GaussianNaiveBayesResult) => {
            if (!gaussianNaiveBayes) {
                return;
            }

            this.gaussianNaiveBayesResult = gaussianNaiveBayes;
        });
        this.refresh();
    }

    refresh() {
        this.store.dispatch(new fromStatisticActions.ComputeGaussianNaiveBayes(this.inputs, this.predict, this.outputs));
    }
}