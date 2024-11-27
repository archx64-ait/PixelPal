# PixelPal | Post Processing

In an HCI Experiment, there are three standard formats of analyzing data:
- wide format
- long format
- mixed format

Since ours was a mixed format design, we nneeded to process our raw survey responses into JASP-ready format to conduct further analyis.

The Jupyter Notebook holds the code to the processing steps as follows:
1. The code handles pre and post-survey responses, performing several key calculations and data transformations. At its core, the script converts Likert scale responses into numeric values. It calculates various mental health assessment scores, including PHQ-9 for depression screening, GAD-7 for anxiety screening, a System Usability Score (SUS), and a Social Connectedness Score. 
2. The script creates unique participant IDs and categorizes participants based on their preferred communication model, avatar-based, or text/voice interactions. The code generates two distinct data formats to facilitate comprehensive statistical analysis: a wide format containing each participant's data in a single row and a long format structured for time-based analysis of repeated measures. 
3. The processed data supports the investigation of three main hypotheses: comparing multimodal AI interactions with text-only interactions, examining the impact of personality traits on interaction effectiveness, and evaluating improvements in mental well-being across different interaction types. 
4. All this processed data is ultimately exported to CSV files for further statistical analysis using JASP software

- Hypothesis H1 was tested using an independent sample t-test to compare SUS scores between conditions, followed by a Mann-Whitney test for robustness. Assumption testing included Shapiro-Wilk for normality and Levene's test for homogeneity of variance, ensuring the statistical validity of the comparisons. 
- Hypothesis H2 was tested using regression analysis to examine the relationship between technology comfort and social connectedness. Durbin-Watson test was used to determine the correlation between observations, and Levene’s tests for homogeneity of variance were used.
- Hypothesis H3 was tested using a two-way Repeated Measures ANOVA test to examine the impact of interaction modality on mental well-being scores.
