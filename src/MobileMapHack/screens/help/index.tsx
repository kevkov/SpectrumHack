import React from "react";
import { StyleSheet } from 'react-native';
import {Container, Text, View, Card, CardItem, Body} from "native-base";
import { HeaderBar } from "../../components/headerBar";

export const Help = (props) => {
    const styles = StyleSheet.create({

        pageHeader: {
            fontWeight: '600',
            padding: 10,
            textDecorationLine: 'underline',
            fontSize: 18,
            textAlign: 'center'
        },
        sectionHeader: {
            fontWeight: '600',
            color: '#ffffff'
        },
        subHeading: {
            textDecorationLine: 'underline',
            paddingBottom: 5
        },
        cardHeader: {
            backgroundColor: '#007aff'
        },
        cardItem: {
        },
        content: {
            padding: 10
        },
        detailItem: {
            padding: 5,
            paddingBottom: 10
        }
    });

    return (
        <Container style={{flex:1}}>
            <HeaderBar navigation={props.navigation} title="Help" showSearch={false} />
            <View style={{
                    alignSelf: 'center',
                    alignItems: 'stretch',
                }}>
                <Text style={styles.pageHeader}>How we calculate your green score and cost</Text>
                <Card>
                    <CardItem bordered style={styles.cardHeader}>
                        <Text style={styles.sectionHeader}>Green Score Calculation</Text>
                    </CardItem>
                    <CardItem style={styles.cardItem}>
                        <Body>
                            <View>
                                <Text style={styles.detailItem}>100 - (Air Quality + Schools)</Text>
                            </View>
                        </Body>
                    </CardItem>
                </Card>
                <Card>
                    <CardItem bordered style={styles.cardHeader}>
                        <Text style={styles.sectionHeader}>Cost Calculation</Text>
                    </CardItem>
                    <CardItem style={styles.cardItem}>
                        <Body>
                            <View>
                            <Text style={styles.detailItem}>(10 - (Green Score x 0.1)) x Distance</Text>
                            </View>
                        </Body>
                    </CardItem>
                </Card>
                <Card>
                    <CardItem bordered style={styles.cardHeader}>
                        <Text style={styles.sectionHeader}>Weightings</Text>
                    </CardItem>
                    <CardItem style={styles.cardItem}>
                        <Body>
                            <View>
                                <Text style={styles.subHeading}>Air Quality</Text>
                                <Text style={styles.detailItem}>20 per average Air Quality Index point</Text>
                                
                                <Text style={styles.subHeading}>Schools</Text>
                                <Text style={styles.detailItem}>40 per school on journey</Text>

                                <Text style={styles.subHeading}>Vehicle Caps</Text>
                                <Text style={styles.detailItem}>Car green score capped at 75</Text>
                            </View>
                        </Body>
                    </CardItem>
                </Card>
            </View>
        </Container>
    )
};